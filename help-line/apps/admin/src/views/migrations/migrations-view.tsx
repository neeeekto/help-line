import React, { useMemo, useState } from 'react';
import { Migration, MigrationStatusType } from '@help-line/entities/admin/api';
import { Card, Drawer, List, Radio, Tabs } from 'antd';
import css from './migrations.module.scss';
import { MigrationRow } from './migration-row';
import { MigrationAction } from './migration-action';
import { MigrationDetails } from './migration-details';
import last from 'lodash/last';

export const MigrationsView: React.FC<{ migrations: Migration[] }> = ({
  migrations,
}) => {
  const noAppliedMigrations = useMemo(
    () => migrations.filter((x) => !x.applied),
    [migrations]
  );
  const appliedMigrations = useMemo(
    () => migrations.filter((x) => x.applied),
    [migrations]
  );

  const activeMigrations = useMemo(
    () =>
      migrations.filter((m) =>
        [MigrationStatusType.Executing, MigrationStatusType.Rollback].includes(
          last(m.statuses)!.$type
        )
      ),
    [migrations]
  );

  const [listType, setListType] = useState('ae');
  const [showDetails, setShowDetails] = useState<Migration | null>(null);
  const list = listType === 'ae' ? noAppliedMigrations : appliedMigrations;
  return (
    <>
      <Card
        className={css.listCard}
        size={'small'}
        title="Migrations"
        extra={
          <Radio.Group
            buttonStyle="solid"
            onChange={(r) => setListType(r.target.value)}
            defaultValue="ae"
            size="small"
          >
            <Radio.Button value="ae">Awaiting & Executing</Radio.Button>
            <Radio.Button value="a">Applied</Radio.Button>
          </Radio.Group>
        }
      >
        <List
          size="small"
          dataSource={list}
          renderItem={(item) => (
            <List.Item key={item.name}>
              <MigrationRow
                migration={item}
                onClick={() => setShowDetails(item)}
              >
                <MigrationAction
                  disabled={activeMigrations.length > 0}
                  migration={item}
                  migrations={migrations}
                />
              </MigrationRow>
            </List.Item>
          )}
        />
      </Card>
      <Drawer
        visible={!!showDetails}
        width="700px"
        drawerStyle={{ overflow: 'auto' }}
        title={showDetails?.name}
        onClose={() => setShowDetails(null)}
      >
        {showDetails && <MigrationDetails migration={showDetails!} />}
      </Drawer>
    </>
  );
};

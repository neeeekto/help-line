import React, { PropsWithChildren } from 'react';
import { FullPageContainer, QuerySimpleLoading } from '@help-line/components';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import css from './templates.module.scss';
import { Resources } from './views/resources';
import { Meta } from './views/meta';
import { Editor } from './views/editor';
import { EditorStoreProvider } from './state';
import { useInitQuery } from './state/hooks';

const TemplatesInitializer = ({ children }: PropsWithChildren) => {
  const initQuery = useInitQuery();
  return <QuerySimpleLoading query={initQuery}>{children}</QuerySimpleLoading>;
};

export const TemplatesView: React.FC = () => {
  return (
    <EditorStoreProvider>
      <TemplatesInitializer>
        <FullPageContainer className={cn(boxCss.flex, boxCss.overflowHidden)}>
          <aside className={cn(css.aside)}>
            <div
              className={cn(
                boxCss.flex,
                boxCss.flexColumn,
                boxCss.fullHeight,
                boxCss.fullWidth,
                boxCss.overflowAuto,
                css.box,
                css.resource
              )}
            >
              <Resources />
            </div>
            <Meta
              className={cn(
                css.box,
                spacingCss.marginTopMd,
                boxCss.flex00Auto,
                boxCss.overflowAuto
              )}
            />
          </aside>
          <Editor />
        </FullPageContainer>
      </TemplatesInitializer>
    </EditorStoreProvider>
  );
};

import React from 'react';
import { FullPageContainer } from '@help-line/components';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import css from '@views/templates/templates.module.scss';
import { Resources } from '@views/templates/views/resources';
import { Meta } from '@views/templates/views/meta';
import { Editor } from '@views/templates/views/editor';

const TemplatesView: React.FC = () => {
  return (
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
  );
};

export default TemplatesView;

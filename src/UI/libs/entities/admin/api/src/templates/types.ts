export interface TemplateBase {
  id: string;
  updatedAt: string; // date
  group?: string;
}

export interface Component extends TemplateBase {
  content: string;
  meta?: unknown;
}

export interface Context extends TemplateBase {
  data: unknown;
  alias?: string;
  extend?: string;
}

export interface Template extends Component {
  contexts: Array<Context['id']>;
  name?: string;
  props?: object;
}

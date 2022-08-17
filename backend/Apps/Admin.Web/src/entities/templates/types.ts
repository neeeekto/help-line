export interface TemplateItem {
  id: string;
  updatedAt: string; // date
  group?: string;
}

export interface Component extends TemplateItem {
  content: string;
  meta?: any;
}

export interface Context extends TemplateItem {
  data: any;
  alias?: string;
  extend?: string;
}

export interface Template extends Component {
  contexts: Array<Context["id"]>;
  name?: string;
  props?: object;
}

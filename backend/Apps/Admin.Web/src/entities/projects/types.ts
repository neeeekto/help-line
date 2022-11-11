export interface ProjectData {
  name: string;
  image?: string;
  languages: string[];
}

export interface CreateProjectData extends ProjectData{
  projectId: string;
}

export interface Project {
  id: string;
  info: {
    name: string;
    image: string;
  };
  languages: string[];
  active: true;
}

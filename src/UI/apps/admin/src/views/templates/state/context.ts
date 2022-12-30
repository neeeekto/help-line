import React, { useContext } from 'react';
import { EditorStore } from './store';

export const EditStoreContext = React.createContext<EditorStore>(null!);

export const useEditStore = () => useContext(EditStoreContext);

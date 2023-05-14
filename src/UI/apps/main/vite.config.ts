import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import ViteTsConfigPathsPlugin from 'vite-tsconfig-paths';
import monacoEditorPlugin from 'vite-plugin-monaco-editor';

export default defineConfig({
  plugins: [
    monacoEditorPlugin({}),
    react(),
    ViteTsConfigPathsPlugin({
      root: '../../',
    }),
  ],
});

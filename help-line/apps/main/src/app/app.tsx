// eslint-disable-next-line @typescript-eslint/no-unused-vars
import styles from './app.module.scss';
import NxWelcome from './nx-welcome';
import { useEffect, useState } from 'react';

export function App() {
  return (
    <>
      <NxWelcome title={'Welcome main'} />
    </>
  );
}

export default App;

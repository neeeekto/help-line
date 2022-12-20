// eslint-disable-next-line @typescript-eslint/no-unused-vars
import styles from './app.module.scss';
import NxWelcome from './nx-welcome';
import { useEffect, useState } from 'react';

export function App() {
  useEffect(() => {
    fetch('/v1/projects/').then(console.log);
  }, []);
  return (
    <>
      <NxWelcome title={'Welcome main'} />
    </>
  );
}

export default App;

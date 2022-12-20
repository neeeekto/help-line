import {
  ChangeEvent,
  Dispatch,
  SetStateAction,
  useCallback,
  useState,
} from 'react';

export const useInput = (
  initial: string = ''
): [
  string,
  (event: ChangeEvent<HTMLInputElement>) => void,
  Dispatch<SetStateAction<string>>
] => {
  const [value, setValue] = useState(initial);
  const onChange = useCallback((event: ChangeEvent<HTMLInputElement>) => {
    setValue(event.target.value);
  }, []);
  return [value, onChange, setValue];
};

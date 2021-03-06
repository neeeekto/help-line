import { ChangeEvent, Dispatch, SetStateAction, useState } from "react";
import { usePersistFn } from "ahooks";

export const useInput = (
  initial: string = ""
): [
  string,
  (event: ChangeEvent<HTMLInputElement>) => void,
  Dispatch<SetStateAction<string>>
] => {
  const [value, setValue] = useState(initial);
  const onChange = usePersistFn((event: ChangeEvent<HTMLInputElement>) => {
    setValue(event.target.value);
  });
  return [value, onChange, setValue];
};

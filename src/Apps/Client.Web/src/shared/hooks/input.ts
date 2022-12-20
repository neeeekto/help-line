import { ChangeEvent, Dispatch, SetStateAction, useState } from "react";
import { useDebounce, usePersistFn } from "ahooks";

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

export const useDebounceInput = (initial = "") => {
  const [search, setSearch] = useState(initial);
  const debouncedSearch = useDebounce(search, { wait: 500 });
  const onSearch = usePersistFn((event: ChangeEvent<HTMLInputElement>) =>
    setSearch(event.target.value)
  );

  return { search, debouncedSearch, onSearch };
};

export const addOrUpdate = <TItem>(
  array: TItem[],
  checker: (item: TItem) => boolean,
  update: (item: TItem) => TItem,
  creator: () => TItem
) => {
  const items = [...array];
  const inx = items.findIndex(checker);
  if (inx === -1) {
    items.push(creator());
  } else {
    items[inx] = update(items[inx]);
  }
  return items;
};

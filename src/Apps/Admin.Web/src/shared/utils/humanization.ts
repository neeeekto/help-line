export function timeout(delay: number) {
  return new Promise((res) => {
    setTimeout(res, delay);
  });
}

export function humanizingTime<T>(prom: Promise<T>): Promise<T> {
  return new Promise<T>((res, rej) => {
    Promise.all([
      prom.then((result) => ({ success: 1, result })).catch((e) => ({ success: 0, error: e })),
      timeout(500),
    ]).then(([result]: any) => {
      if (result.success) {
        res(result.result);
      } else {
        rej(result.error);
      }
    });
  });
}

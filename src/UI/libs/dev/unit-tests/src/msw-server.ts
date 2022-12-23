import { setupServer } from 'msw/node';

export const setupMSW = () => {
  const server = setupServer();
  beforeAll(() => {
    server.listen();
  });

  afterEach(() => {
    server.resetHandlers();
  });

  afterAll(() => {
    server.close();
  });

  return server;
};

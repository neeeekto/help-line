import { setupServer } from 'msw/node';
import { RequestHandler } from 'msw';

export const setupMSW = (...defaultHandlers: Array<RequestHandler>) => {
  const server = setupServer();
  beforeAll(() => {
    server.listen();
  });

  afterEach(() => {
    server.resetHandlers(...defaultHandlers);
  });

  afterAll(() => {
    server.close();
  });

  return server;
};

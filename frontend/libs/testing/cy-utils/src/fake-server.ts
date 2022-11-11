import {RequestHandler, setupWorker} from 'msw';

export const worker = setupWorker();

export const startMockServer = () => {
  cy.wrap(worker.start({
    onUnhandledRequest: 'bypass' // ignore unknown requests
  }), {log: false}).log("Mock server started");
};

export const stopMockServer = () => {
  worker.stop();
  cy.log("Mock server stopped");
};

export const setupMockServer = (...handlers: RequestHandler[]) => {
  startMockServer();
  worker.resetHandlers(...handlers);
}


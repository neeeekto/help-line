import { HttpStubRequest } from './create-stub-api';
import { RouteHandler } from 'cypress/types/net-stubbing';

declare var cy: any;

export namespace CyApiHandlers {
  const makeHandler = (response: RouteHandler) => (req: HttpStubRequest) => {
    return cy.intercept(
      {
        method: req.method,
        url: req.url,
        headers: req.headers,
        query: req.params,
      },
      response
    );
  };

  export const success = (result: any, code = 200) =>
    makeHandler({
      statusCode: code,
      body: result,
    });

  export const error = (code: number, error?: any) =>
    makeHandler({
      statusCode: code,
    });

  export const delay = (duration: number) =>
    makeHandler({
      delay: duration,
    });
}

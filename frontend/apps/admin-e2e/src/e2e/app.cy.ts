import { jobsFakeApi, jobStubFactory } from '@help-line/stub/admin';
import {makeSuccessResponse} from '@help-line/stub/share';
import {setupMockServer} from "@help-line/testing/cy-utils";

describe('admin', () => {

  beforeEach(() => {
    setupMockServer(jobsFakeApi.get(makeSuccessResponse([jobStubFactory.createJob()])))
    cy.visit('/');
  });

  it('should display welcome message', () => {
    // Custom command example, see `../support/commands.ts` file
    cy.get('#root');
  });
  it('should display welcome message2', () => {
    // Custom command example, see `../support/commands.ts` file
    cy.get('#root').should('contain.text', '1');
  });
});

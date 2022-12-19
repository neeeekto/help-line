import { getGreeting } from '../support/app.po';
import { adminProjectsStubApi } from '@help-line/entities/admin/stubs';
import { CyApiHandlers } from '@help-line/dev/http-stubs';

describe('main', () => {
  beforeEach(() => {
    cy.visit('/');
    adminProjectsStubApi
      .get()
      .handle(CyApiHandlers.success([1, 2, 3]))
      .as('stubApi');
  });

  it('should display welcome message', () => {
    // Custom command example, see `../support/commands.ts` file
    cy.login('my-email@something.com', 'myPassword');

    // Function helper example, see `../support/app.po.ts` file
    getGreeting().contains('Welcome main');
  });
});

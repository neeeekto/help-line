import { stubClient } from './stub-client';

describe('stubClient', () => {
  it('should work', () => {
    expect(stubClient()).toEqual('stub-client');
  });
});

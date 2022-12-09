import { render } from '@testing-library/react';

import { AuthProvider } from './auth-provider';

describe('AuthProvider', () => {
  it('should render successfully', () => {
    const { baseElement } = render(<AuthProvider settings={{}} />);
    expect(baseElement).toBeTruthy();
  });

  it('should render children', () => {
    const { getByText } = render(
      <AuthProvider settings={{}} children={<div>test</div>} />
    );
    expect(getByText('test')).toBeTruthy();
  });
});

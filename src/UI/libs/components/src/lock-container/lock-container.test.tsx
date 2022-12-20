import { render } from '@testing-library/react';

import LockContainer from './lock-container';

describe('LockContainer', () => {
  it('should render successfully', () => {
    const { baseElement } = render(<LockContainer />);
    expect(baseElement).toBeTruthy();
  });

  it('should show children', () => {
    const { getByText } = render(<LockContainer children={<div>test</div>} />);
    expect(getByText('test')).toBeTruthy();
  });

  it('should show spin if children are null', () => {
    const { getByText } = render(<LockContainer text="test" />);
    expect(getByText('test')).toBeTruthy();
  });
});

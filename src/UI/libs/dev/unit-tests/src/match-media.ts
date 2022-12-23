import MatchMediaMock from 'jest-matchmedia-mock';

export const setupMatchMedia = () => {
  let matchMedia: MatchMediaMock;
  beforeAll(() => {
    matchMedia = new MatchMediaMock();
  });

  afterEach(() => {
    matchMedia.clear();
  });

  return {
    getMatchMedia: () => matchMedia,
  };
};

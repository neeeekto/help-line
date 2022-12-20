import { AxiosInstance } from 'axios';
import { anything, instance, mock, verify, when } from 'ts-mockito';
import axios from 'axios';
import { AxiosHttpBackend } from './axios.http-backend';

describe('AxiosHttpBackend', () => {
  const createMock = jest.fn();
  axios.create = createMock;

  beforeEach(() => {
    createMock.mockClear();
  });

  it('call axios function', () => {
    new AxiosHttpBackend('');
    expect(axios.create).toBeCalled();
  });

  it('set axios baseUrl', () => {
    const baseUrl = 'test';
    new AxiosHttpBackend(baseUrl);
    expect(createMock).toBeCalledWith(
      expect.objectContaining({ baseURL: baseUrl })
    );
  });

  it('use axios request', () => {
    const baseUrl = 'test';
    const axios = mock<AxiosInstance>();
    createMock.mockReturnValue(instance(axios));
    when(axios.request(anything())).thenResolve({} as any);
    const backend = new AxiosHttpBackend(baseUrl);
    backend.handle({});

    verify(axios.request(anything())).once();
  });
});

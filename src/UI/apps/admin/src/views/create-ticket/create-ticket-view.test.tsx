import { CreateTicketView } from './create-ticket-view';
import { ComponentProps } from 'react';
import {
  render,
  act,
  waitFor,
  fireEvent,
  within,
} from '@testing-library/react';
import userEvents from '@testing-library/user-event';
import { setupMatchMedia, setupMSW } from '@help-line/dev/unit-tests';
import { expect } from '@storybook/jest';
import {
  adminProjectsStubApi,
  AdminProjectStubs,
} from '@help-line/entities/admin/stubs';
import { MswHandlers } from '@help-line/dev/http-stubs';
import { UnitTestWrapper } from '../../unit-test-wrapper';

describe('create-ticket-view', () => {
  setupMatchMedia();
  const msw = setupMSW(
    adminProjectsStubApi
      .get()
      .handle(MswHandlers.success([AdminProjectStubs.createProject()]))
  );

  const renderComponent = (args?: ComponentProps<typeof CreateTicketView>) =>
    render(
      <UnitTestWrapper>
        <CreateTicketView {...args} />
      </UnitTestWrapper>
    );
  describe('rendering', () => {
    it('should show submit button', () => {
      const { getByText } = renderComponent();
      expect(getByText(/Create$/).closest('button')).toBeInTheDocument();
    });
    it('should show add channel button', () => {
      const { getByTestId } = renderComponent();
      expect(getByTestId('add_channel').closest('button')).toBeInTheDocument();
    });
    it('should show delete channel button', () => {
      const { getByTestId } = renderComponent();
      act(() => {
        userEvents.click(getByTestId('add_channel'));
      });
      expect(getByTestId('delete_0').closest('button')).toBeInTheDocument();
    });
    it('should show add meta button', () => {
      const { getByTestId } = renderComponent();
      expect(getByTestId('add_meta').closest('button')).toBeInTheDocument();
    });
    it('should show delete meta button', () => {
      const { getByTestId, queryByText } = renderComponent();
      act(() => {
        userEvents.click(getByTestId('add_meta'));
      });
      expect(getByTestId('delete_0').closest('button')).toBeInTheDocument();
    });
    it('should show form error', async () => {
      const { getByText, getByTestId } = renderComponent();
      act(() => {
        fireEvent.submit(getByTestId('submit-form'));
      });
      await waitFor(() =>
        expect(getByText(/Please select project/gi)).toBeInTheDocument()
      );
    });
  });
  describe('interactions', () => {
    it('should use selected project languages in selector', async () => {
      const lang = 'my-test-language-str';
      const project = AdminProjectStubs.createProject({
        languages: [lang],
      });
      msw.resetHandlers(
        adminProjectsStubApi.get().handle(MswHandlers.success([project]))
      );
      const { getByText, getByTestId } = renderComponent();
      act(() => {
        fireEvent.mouseDown(
          within(getByTestId('project-selector')).getByRole('combobox')
        );
      });

      await waitFor(() =>
        expect(getByText(project.info.name)).toBeInTheDocument()
      );
      act(() => {
        fireEvent.click(getByText(project.info.name));
      });
      fireEvent.mouseDown(
        within(getByTestId('language-selector')).getByRole('combobox')
      );
      await waitFor(() => expect(getByText(lang)).toBeInTheDocument());
    });
    it('should clear form after successful submit', () => {
      // TODO
    });
    it('should submit valid data', async () => {
      // TODO
    });
    it('should not submit invalid data', () => {});
  });
  describe('lifecycle', () => {
    it('should load projects for selector', async () => {
      const project = AdminProjectStubs.createProject();
      msw.resetHandlers(
        adminProjectsStubApi.get().handle(MswHandlers.success([project]))
      );
      const { getByText, getByTestId } = renderComponent();
      act(() => {});
      fireEvent.mouseDown(
        within(getByTestId('project-selector')).getByRole('combobox')
      );
      await waitFor(() =>
        expect(getByText(project.info.name)).toBeInTheDocument()
      );
    });
  });
});

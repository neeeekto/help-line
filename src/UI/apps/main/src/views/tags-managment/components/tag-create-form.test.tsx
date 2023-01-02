import '@test-utils/mocks/match-media.mock';
import { TagCreateForm } from './tag-create-form';
import { render, screen, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { HelpdeskTagsClientStubs } from '@help-line/entities/client/stubs';

describe('TagForm', () => {
  const tag = HelpdeskTagsClientStubs.createTag();
  describe('rendering', () => {
    it('can rendered', () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
    });

    it('disable form if loading', async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={true}
        />
      );
      screen.getAllByRole('button').forEach((x) => expect(x).toBeDisabled());
      screen.getAllByRole('textbox').forEach((x) => expect(x).toBeDisabled());
    });

    it('can set loading', async () => {
      const { rerender } = render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      screen.getAllByRole('textbox').forEach((x) => expect(x).toBeEnabled());
      rerender(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={true}
        />
      );
      screen.getAllByRole('textbox').forEach((x) => expect(x).toBeDisabled());
    });
  });

  describe('interactions', () => {
    it('can change form', async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      userEvent.type(screen.getByRole('textbox'), 'test1');
      userEvent.click(screen.getByTestId('add'));
      expect(screen.queryByTestId('tag')).toBeTruthy();
    });
    it('cannot change form if loading', async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={true}
        />
      );
      userEvent.type(screen.getByRole('textbox'), 'test1');
      userEvent.click(screen.getByTestId('add'));
      expect(screen.queryByTestId('tag')).toBeFalsy();
    });

    it("can input multi values with ',' splitter", async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      userEvent.type(screen.getByRole('textbox'), 'test1,test2');
      userEvent.click(screen.getByTestId('add'));
      expect(screen.queryAllByTestId('tag').length).toBe(2);
    });

    it("can input multi values with ';' splitter", async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      userEvent.type(screen.getByRole('textbox'), 'test1; test2');
      userEvent.click(screen.getByTestId('add'));
      expect(screen.queryAllByTestId('tag').length).toBe(2);
    });

    it('can input fist value without second', async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      userEvent.type(screen.getByRole('textbox'), 'test1;,');
      userEvent.click(screen.getByTestId('add'));
      expect(screen.queryAllByTestId('tag').length).toBe(1);
    });

    it('can input exist value', async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      userEvent.type(screen.getByRole('textbox'), tag.key);
      userEvent.click(screen.getByTestId('add'));
      expect(screen.queryAllByTestId('tag').length).toBe(1);
    });

    it('can input value by enter', async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      userEvent.type(screen.getByRole('textbox'), 'test_test');
      fireEvent.keyDown(screen.getByRole('textbox'), {
        key: 'Enter',
        code: 'Enter',
        charCode: 13,
      });
      expect(screen.queryAllByTestId('tag').length).toBe(1);
    });

    it('form ignore empty value', async () => {
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={() => {}}
          onCancel={() => {}}
          loading={false}
        />
      );
      userEvent.type(screen.getByRole('textbox'), '   ');
      userEvent.click(screen.getByTestId('add'));
      expect(screen.queryAllByTestId('tag').length).toBe(0);
    });

    it('exist value cannot be saved', async () => {
      let data: string[] = [];
      render(
        <TagCreateForm
          tags={[tag]}
          onSave={(res) => {
            data = res;
          }}
          onCancel={() => {}}
          loading={false}
        />
      );
      const newTag = 'test2';
      userEvent.type(screen.getByRole('textbox'), [tag.key, newTag].join(','));
      userEvent.click(screen.getByTestId('add'));
      userEvent.click(screen.getByTestId('save'));
      expect(data.includes(tag.key)).toBeFalsy();
      expect(data.includes(newTag)).toBeTruthy();
    });
  });
  describe('lifecycle', () => {});
});

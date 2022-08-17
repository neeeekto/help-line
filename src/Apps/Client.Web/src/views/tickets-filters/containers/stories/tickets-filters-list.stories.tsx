import React from "react";
import { ComponentMeta } from "@storybook/react";
import { TicketsFiltersList } from "../tickets-filters-list";
import {
  makeTicketsFilterApi,
  TicketFilter,
  TicketFilterFeatures,
} from "@entities/helpdesk/tickets";
import { StoryBookRoot } from "@test-utils/story-book-root";
import { ApiStubsRoot } from "@test-utils/api.stub";
import { makeTicketFilterTestData } from "@test-utils/data/ticket-filter.test-data";
import { makeOperatorApi } from "@entities/helpdesk/operators";
import { makeProjectTestData } from "@test-utils/data/project.test-data";
import { makeOperatorViewTestData } from "@test-utils/data/operator.test-data";

export default {
  title: "Views/Tickets Filters",
  component: TicketsFiltersList,
  argTypes: {},
} as ComponentMeta<typeof TicketsFiltersList>;

const project = makeProjectTestData();
const user1 = makeOperatorViewTestData();
const user2 = makeOperatorViewTestData();
const user3 = makeOperatorViewTestData();
const user4 = makeOperatorViewTestData({
    firstName: "LongNameUserFirstName",
    lastName: "LongNameUserLastName"
});

const TemplateFactory =
  (apiStub: Partial<ReturnType<typeof makeTicketsFilterApi>>) => () =>
    (
      <StoryBookRoot project={project} me={user1}>
        <ApiStubsRoot>
          <ApiStubsRoot.Stub apiFactory={makeTicketsFilterApi} stub={apiStub} />
          <ApiStubsRoot.Stub
            apiFactory={makeOperatorApi}
            stub={{
              getListView: () =>
                Promise.resolve().then(() => [
                  user1,
                  user2,
                  user3,
                    user4,
                  makeOperatorViewTestData(),
                ]),
            }}
          />
          <TicketsFiltersList />
        </ApiStubsRoot>
      </StoryBookRoot>
    );

export const Loading = TemplateFactory({
  get: async () => new Promise(() => {}),
});

export const Error = TemplateFactory({
  get: async () => Promise.reject("Error"),
});

export const Empty = TemplateFactory({
  get: async () => Promise.resolve().then(() => [] as TicketFilter[]),
});

export const List = TemplateFactory({
  delete: (entityId) =>
    new Promise((res) => {
      setTimeout(res, 400);
    }),
  get: async () =>
    Promise.resolve().then(
      () =>
        [
          makeTicketFilterTestData({
            owner: void 0,
            features: [TicketFilterFeatures.Important],
          }),
          makeTicketFilterTestData({
            owner: user2.id,
          }),
            makeTicketFilterTestData({
            owner: user4.id,
          }),
          makeTicketFilterTestData({
            features: [TicketFilterFeatures.Automations],
            owner: user1.id,
          }),
          makeTicketFilterTestData({
            features: [TicketFilterFeatures.Important],
            owner: user2.id,
          }),
          makeTicketFilterTestData({
            features: [
              TicketFilterFeatures.Automations,
              TicketFilterFeatures.Important,
            ],
            owner: user1.id,
          }),
          makeTicketFilterTestData({
            features: [TicketFilterFeatures.Important],
            owner: user1.id,
            share: {
              $type: "TicketFilterShareGlobal",
            },
          }),
          makeTicketFilterTestData({
            features: [TicketFilterFeatures.Important],
            owner: user1.id,
            share: {
              $type: "TicketFilterShareForOperators",
              operators: [user2.id, user1.id, user3.id],
            },
          }),
          makeTicketFilterTestData({
            features: [
              TicketFilterFeatures.Automations,
              TicketFilterFeatures.Important,
            ],
            owner: user1.id,
            share: {
              $type: "TicketFilterShareForOperators",
              operators: [user2.id, user1.id, user3.id],
            },
          }),
        ] as TicketFilter[]
    ),
});

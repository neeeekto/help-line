import { TicketFilter, TicketFilterFeatures } from "@entities/helpdesk/tickets";
import { FieldFilterOperators } from "@entities/filter";
import { faker } from "@faker-js/faker";

export const makeTicketFilterTestData = (
  data: Partial<TicketFilter> = {}
): TicketFilter => ({
  id: faker.datatype.uuid(),
  name: faker.lorem.paragraph(),
  changed: faker.datatype.datetime({ max: Date.now() }).toDateString(),
  features: [],
  owner: faker.datatype.uuid(),
  filter: {
    $type: "ValueFilter",
    operator: FieldFilterOperators.Equal,
    path: [""],
    value: {
      $type: "ConstantFilterValue",
      value: "",
    },
  },
  ...data,
});

import { ValidatorRule } from "rc-field-form/lib/interface";
import { CreateTicketRequest } from "@entities/helpdesk";

export const userChannelsValidator: ValidatorRule["validator"] = (
  rule,
  value: CreateTicketRequest["channels"],
  callback
) => {
  if (value.some((x) => !x.userId || !x.channel)) {
    callback("Error");
    return;
  }
  callback();
};

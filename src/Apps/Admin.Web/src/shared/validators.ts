import { ValidatorRule } from "rc-field-form/lib/interface";

export const notEmptyArrayValidator: ValidatorRule["validator"] = async (
  rule,
  value,
  callback
) => {
  if (!value || !value.length) {
    return Promise.reject(new Error("Required"));
  }
};

import { ValidatorRule } from 'rc-field-form/lib/interface';
import { CreateTicketRequest } from '@help-line/entities/admin/api';

export const userChannelsValidator: ValidatorRule['validator'] = (
  rule,
  value: CreateTicketRequest['channels'],
  callback
) => {
  if (value.some((x) => !x.userId || !x.channel)) {
    callback('Error');
    return;
  }
  callback();
};

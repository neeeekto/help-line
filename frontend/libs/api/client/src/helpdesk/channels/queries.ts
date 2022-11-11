import { Ticket } from "@entities/helpdesk/tickets";
import { useQuery } from "react-query";
import { channelApi } from "./api";

const queryKeys = {
  root: "channels",
  preview: "preview",
  feedback: "feedback",
  messages: "messages",
};
export default queryKeys;

export const useTicketFeedbackPreviewQuery = (
  ticketId: Ticket["id"],
  feedbackId: string
) =>
  useQuery(
    [
      queryKeys.root,
      queryKeys.preview,
      ticketId,
      queryKeys.feedback,
      feedbackId,
    ],
    () => channelApi.getFeedbackPreview(ticketId, feedbackId)
  );

export const useTicketMessagesPreviewQuery = (ticketId: Ticket["id"]) =>
  useQuery(
    [queryKeys.root, queryKeys.preview, ticketId, queryKeys.messages],
    () => channelApi.getMessagesPreview(ticketId)
  );

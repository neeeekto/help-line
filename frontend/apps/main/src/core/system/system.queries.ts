import { useMutation, useQuery, useQueryClient } from "react-query";
import { systemApi } from "./system.api";
import { Message, MessageData } from "./system.types";

const keys = {
  root: "system",
  messages: "messages",
};

export const useSystemSettingsQuery = () =>
  useQuery([keys.root, "settings"], systemApi.getSettings);

export const useAppStateQuery = () =>
  useQuery([keys.root, "app"], systemApi.getState, {
    refetchInterval: 1000 * 60 * 10, // 10min
  });

export const useMessagesQuery = (all = false) =>
  useQuery([keys.root, keys.messages, all], () => systemApi.getMessages(all), {
    refetchInterval: all ? false : 1000 * 60 * 10, // 10min
  });

export const useAddMessageMutation = () => {
  const client = useQueryClient();
  useMutation(
    [keys.root, keys.messages, "add"],
    (data: MessageData) => systemApi.addMessage(data),
    {
      onSuccess: (result, args, ctx) =>
        client.invalidateQueries([keys.root, keys.messages]),
    }
  );
};

export const useUpdateMessageMutation = (messageId: Message["id"]) => {
  const client = useQueryClient();
  useMutation(
    [keys.root, keys.messages, "update", messageId],
    (data: MessageData) => systemApi.updateMessage(messageId, data),
    {
      onSuccess: (result, args, ctx) =>
        client.invalidateQueries([keys.root, keys.messages]),
    }
  );
};

export const useDeleteMessageMutation = (messageId: Message["id"]) => {
  const client = useQueryClient();
  useMutation(
    [keys.root, keys.messages, "delete", messageId],
    () => systemApi.deleteMessage(messageId),
    {
      onSuccess: (result, args, ctx) =>
        client.invalidateQueries([keys.root, keys.messages]),
    }
  );
};

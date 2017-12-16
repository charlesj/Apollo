import { Position, Toaster } from "@blueprintjs/core";

export const Notifier = Toaster.create({
  className: "my-toaster",
  position: Position.TOP,
});

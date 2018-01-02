import { metaActions } from "../redux/actions";
import store from "../redux";

export const NotifySuccess = message => {
  store.dispatch(metaActions.notify({ type: "success", message }));
};

export const NotifyError = message => {
  store.dispatch(metaActions.notify({ type: "error", message }));
};

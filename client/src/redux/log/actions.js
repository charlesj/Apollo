import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";

const actionCreators = createActions({
  log: {
    save: basicActions(),
    load: basicActions()
  }
});

const actions = actionCreators.log;

export default actions;

export function load({ start }) {
  return dispatchBasicActions(actions.load, async () => {
    const payload = await apolloServer.invoke("getLogEntries", {
      start
    });
    return payload;
  });
}

export function save(entry) {
  return dispatchBasicActions(actions.save, async () => {
    const updated = await apolloServer.invoke("addLogEntry", { ...entry });

    return { entry: updated };
  });
}

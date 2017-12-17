import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";

const actionCreators = createActions({
  summaries: {
    getSummaries: basicActions(),
  }
});

const actions = actionCreators.summaries;

export default actions;

export function getSummaries() {
  return dispatchBasicActions(actions.getSummaries, async () => {
    const summaries = await apolloServer.invoke("getSummaries", {});
    return { summaries };
  });
}

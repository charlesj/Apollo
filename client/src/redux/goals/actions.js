import { createActions } from "redux-actions";
import { basicActions, dispatchBasicActions } from "../redux-helpers";
import apolloServer from "../../services/apolloServer";

const actionCreators = createActions({
  goals: {
    getGoals: basicActions(),
    upsertGoal: basicActions()
  }
});

const actions = actionCreators.goals;

export default actions;

export function getGoals() {
  return dispatchBasicActions(actions.getGoals, async () => {
    const goals = await apolloServer.invoke("getGoals", {});
    if (Array.isArray(goals)) {
      return { goals };
    } else {
      return { goals: [] };
    }
  });
}

export function upsertGoal(goal) {
  return dispatchBasicActions(actions.upsertGoal, async () => {
    await apolloServer.invoke("upsertGoal", { goal });
    return { goal };
  });
}

export function byName(state, name) {
  return state.metrics.metrics.filter(m => m.name === name);
}

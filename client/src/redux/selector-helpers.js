export function keyedIdToArray(obj) {
  return Object.keys(obj).map(key => {
    return obj[key];
  });
}

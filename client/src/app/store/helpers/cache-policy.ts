
// TODO: Re-write
export function lastFetchedNow() {
	return Date.now();
}

/**
 * Extend from this interface or used the mapped type StateItem to add tracking time to entities.
 */
export interface TrackedState {
	/**
	 * Milliseconds elapsed since the UNIX epoch when item was fetched from api.
	 */
	lastFetched: number;
}

export const expiry = {
	short: 60000, // 1 min
	default: 600000, // 10 mins
	long: 1800000 // 30 mins
};

/**
 * Determines if a state item should be refreshed from the API.
 * @param stateItem item to check
 * @param expireMs number of ms since the last fetch after which this item expires.
 */
export function isStale<T>(stateItem: TrackedState | undefined, expireMs: number = expiry.default): boolean {
	if (!stateItem) return true;
	const expired = (stateItem.lastFetched + expireMs) < lastFetchedNow();
	return expired;
}

/**
 * Saves negating isStale()!
 * @param stateItem item to check
 * @param expireMs number of ms since the last fetch after which this item expires.
 */
export function isFresh<T>(stateItem: TrackedState | undefined, expireMs: number = expiry.default): boolean {
	return !isStale(stateItem, expireMs);
}

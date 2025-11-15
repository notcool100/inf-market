export { matchers } from './matchers.js';

export const nodes = [
	() => import('./nodes/0'),
	() => import('./nodes/1'),
	() => import('./nodes/2'),
	() => import('./nodes/3'),
	() => import('./nodes/4'),
	() => import('./nodes/5'),
	() => import('./nodes/6'),
	() => import('./nodes/7'),
	() => import('./nodes/8'),
	() => import('./nodes/9'),
	() => import('./nodes/10'),
	() => import('./nodes/11'),
	() => import('./nodes/12'),
	() => import('./nodes/13'),
	() => import('./nodes/14'),
	() => import('./nodes/15'),
	() => import('./nodes/16'),
	() => import('./nodes/17')
];

export const server_loads = [];

export const dictionary = {
		"/": [2],
		"/brand/campaigns": [3],
		"/brand/campaigns/create": [6],
		"/brand/campaigns/[id]": [4],
		"/brand/campaigns/[id]/edit": [5],
		"/brand/dashboard": [7],
		"/influencers": [12],
		"/influencers/[id]": [13],
		"/influencer/campaigns/[id]/deliverables": [8],
		"/influencer/dashboard": [9],
		"/influencer/profile/create": [10],
		"/influencer/profile/edit": [11],
		"/login": [14],
		"/payments": [15],
		"/register": [16],
		"/wallet": [17]
	};

export const hooks = {
	handleError: (({ error }) => { console.error(error) }),
};

export { default as root } from '../root.svelte';
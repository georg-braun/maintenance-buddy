import axios from 'axios';
import auth from '../auth-service';

import { env } from '$env/dynamic/public';

const serverUrl = env.PUBLIC_API_SERVER_ADDRESS;


export async function makeGetRequest(route) {
	try {
		const token = await auth.getAccessToken();

		const config = {
			url: `${serverUrl}/api/${route}`,
			method: 'GET',
			headers: {
				'content-type': 'application/json',
				Authorization: `Bearer ${token}`
			}
		};

		try {
			return axios.get(`${serverUrl}/api/${route}`, config);
		} catch (error) {
			console.log(`${error.response.status}: ${error.response.data}`);
			return error.response;
		}
		
	} catch (error) {
		console.log(error);
	}
}



export async function sendPost(endpoint, data) {
	try {
		const token = await auth.getAccessToken();
		const config = {
			url: `${serverUrl}/api/${endpoint}`,
			method: 'POST',
			headers: {
				'content-type': 'application/json',
				Authorization: `Bearer ${token}`
			}
		};

		try {
			const response = await axios.post(`${serverUrl}/api/${endpoint}`, data, config);
			return response;
		} catch (error) {
			console.log(`${error.response.status}: ${error.response.data}`);
			return error.response;
		}
	} catch (error) {
		console.log(error);
	}
}

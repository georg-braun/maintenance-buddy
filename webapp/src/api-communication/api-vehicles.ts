import { makeGetRequest, sendPost } from './api-service';

export async function getVehicles() {
	try {
		const response = await makeGetRequest('get-vehicles');
		return response.data;
	} catch (error) {
		console.log(error);
	}
}

export async function getActionTemplates(vehicleId) {
	try {
		const response = await makeGetRequest(`get-action-templates/?vehicleId=${vehicleId}`);
		return response.data;
	} catch (error) {
		console.log(error);
	}
}

export async function getActions(vehicleId) {
	try {
		const response = await makeGetRequest(`get-action-templates/?vehicleId=${vehicleId}`);
		return response.data;
	} catch (error) {
		console.log(error);
	}
}

export async function createVehicle(name, kilometer) {
	const data = {
		Name: name,
		Kilometer: kilometer
	};
	const response = await sendPost('create-vehicle', data);
	if (response.status === 201 || response.status === 200) console.log('vehicle created');
}

export async function addActionTemplate(vehicleId, name, kilometerInterval, timeInterval) {
	const data = {
		VehicleId: vehicleId,
		Name: name,
		KilometerInterval: kilometerInterval
	};
	const response = await sendPost('add-action-template', data);
	if (response.status === 201 || response.status === 200) console.log('action template added');
}

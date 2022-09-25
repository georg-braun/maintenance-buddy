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
		const response = await makeGetRequest(`get-actions-of-vehicle/?vehicleId=${vehicleId}`);
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

export async function addAction(vehicleId, actionTemplateId, date, kilometer, note) {
	const data = {
		VehicleId: vehicleId,
		ActionTemplateId: actionTemplateId,
		Date: date,
		Kilometer: kilometer,
		Note: note
	};
	const response = await sendPost('add-action', data);
	if (response.status === 201 || response.status === 200) console.log('action template added');
}
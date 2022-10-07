import { makeGetRequest, sendPost } from './api-service';
import FakeServer from "./api-vehicles-faker"

const fakeData = true;
const fakeServer = new FakeServer()
export async function getVehicles() {
	return fakeServer.getVehicles();
}

export async function getActionTemplates(vehicleId) {
	return fakeServer.getActionTemplates(vehicleId);
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
	fakeServer.createVehicle(name, kilometer)
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
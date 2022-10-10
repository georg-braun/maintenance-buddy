import FakeServer from './api-vehicles-faker';
import ApiServer from './api-vehicles-http';

const server = new ApiServer(); //new FakeServer()
export async function getVehicles() {
	return server.getVehicles();
}

export async function getActionTemplates(vehicleId) {
	return server.getActionTemplates(vehicleId);
}

export async function getVehicle(vehicleId) {
	return server.getVehicle(vehicleId);
}

export async function getActions(vehicleId) {
	return server.getActions(vehicleId);
}

export async function createVehicle(name, kilometer) {
	server.createVehicle(name, kilometer);
}

export async function addActionTemplate(vehicleId, name, kilometerInterval, timeInterval) {
	server.addActionTemplate(vehicleId, name, kilometerInterval, timeInterval);
}

export async function addAction(vehicleId, actionTemplateId, date, kilometer, note) {
	server.addAction(vehicleId, actionTemplateId, date, kilometer, note);
}

export async function deleteAction(vehicleId, actionTemplateId, actionId) {
	server.deleteAction(vehicleId, actionTemplateId, actionId);
}

export async function changeVehicleName(vehicleId, name) {
	console.log(`change vehicle name to ${name}`);
	server.changeVehicleName(vehicleId, name);
}

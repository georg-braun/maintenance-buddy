import FakeServer from "./api-vehicles-faker"

const fakeData = true;
const fakeServer = new FakeServer()
export async function getVehicles() {
	return fakeServer.getVehicles();
}

export async function getActionTemplates(vehicleId) {
	return fakeServer.getActionTemplates(vehicleId);
}

export async function getVehicleSummary(vehicleId){
	return fakeServer.getVehicleSummary(vehicleId);
}

export async function getActions(vehicleId) {

}

export async function createVehicle(name, kilometer) {

}

export async function addActionTemplate(vehicleId, name, kilometerInterval, timeInterval) {

}

export async function addAction(vehicleId, actionTemplateId, date, kilometer, note) {
}
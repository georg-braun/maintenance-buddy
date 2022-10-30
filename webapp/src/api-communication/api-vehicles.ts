import FakeServer from './api-vehicles-faker';
import ApiServer from './api-vehicles-http';

const server = new ApiServer(); //new FakeServer()
export async function getVehicles() {
	return server.getVehicles();
}

export async function getActionTemplates(vehicleId): Promise<void> {
	return server.getActionTemplates(vehicleId);
}

export async function getVehicle(vehicleId) : Promise<void>{
	return server.getVehicle(vehicleId);
}

export async function getActions(vehicleId): Promise<void> {
	return server.getActions(vehicleId);
}

export async function createVehicle(name, kilometer): Promise<void> {
	return server.createVehicle(name, kilometer);
}

export async function addActionTemplate(vehicleId, name, kilometerInterval, timeInterval) : Promise<void>{
	return server.addActionTemplate(vehicleId, name, kilometerInterval, timeInterval);
}

export async function deleteSchedule(vehicleId, scheduleId): Promise<void> {
	return server.deleteSchedule(vehicleId, scheduleId);
}
export async function changeScheduleName(vehicleId, scheduleId, name): Promise<void> {
	return server.changeScheduleName(vehicleId, scheduleId, name);
}
export async function changeScheduleKilometerInterval(vehicleId, scheduleId, kilometer) : Promise<void>{
	return server.changeScheduleKilometerInterval(vehicleId, scheduleId, kilometer);
}
export async function changeScheduleTimeInterval(vehicleId, scheduleId, days): Promise<void> {
	return server.changeScheduleTimeInterval(vehicleId, scheduleId, days);
}

export async function addAction(vehicleId, actionTemplateId, date, kilometer, note): Promise<void> {
	return server.addAction(vehicleId, actionTemplateId, date, kilometer, note);
}
export async function changeActionNote(vehicleId, actionTemplateId, actionId, note) : Promise<void>{
	return server.changeActionNote(vehicleId, actionTemplateId, actionId, note);
}
export async function changeActionDate(vehicleId, actionTemplateId, actionId, date) : Promise<void>{
	return server.changeActionDate(vehicleId, actionTemplateId, actionId, date);
}
export async function changeActionKilometer(vehicleId, actionTemplateId, actionId, kilometer) : Promise<void>{
	return server.changeActionKilometer(vehicleId, actionTemplateId, actionId, kilometer);
}

export async function deleteAction(vehicleId, actionTemplateId, actionId): Promise<void> {
	return server.deleteAction(vehicleId, actionTemplateId, actionId);
}

export async function deleteVehicle(vehicleId) : Promise<void> {
	return server.deleteVehicle(vehicleId);
}

export async function changeVehicleName(vehicleId, name) : Promise<void> {
	return server.changeVehicleName(vehicleId, name);
}

export async function changeVehicleKilometer(vehicleId, kilometer) : Promise<void> {
	return server.changeVehicleKilometer(vehicleId, kilometer);
}

export async function getPendingActions(vehicleId) : Promise<void> {
	return server.getPendingActions(vehicleId);
}
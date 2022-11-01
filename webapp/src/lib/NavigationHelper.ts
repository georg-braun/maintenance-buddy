import { goto } from '$app/navigation';

const base = window.location.origin

function gotoInternal(path){
    let route = base + "/" + path
    console.log(route)
    return goto(route)
}

export function goToAllVehicles(){
    return gotoInternal("")
}

export function goToVehicle(vehicleId){
    return gotoInternal(`vehicles/${vehicleId}`)
}

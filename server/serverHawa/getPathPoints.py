import requests
import warnings
import stations

warnings.filterwarnings("ignore")

#get all points on multiple paths starting froma source to a destination
def getpoints(source_lat=25.2627964,source_longi=82.9895458,dest_lat=25.2949006,dest_longi=82.9503778):
    ty = requests.get("https://maps.googleapis.com/maps/api/directions/json?origin="+str(source_lat)+","+str(source_longi)+"&destination="+str(dest_lat)+","+str(dest_longi)+"&alternatives=true&key=AIzaSyDkfhH-xWjeINi4WLnH4IPqeo6YOil1rtY")
    json_obj = ty.json()
    print json_obj
    route_ = json_obj['routes']
    len_route_ = len(route_)
    routes = []
    for i in range(len_route_):
        sub_ = []
        x = route_[i]
        x = x[u'legs'][0]
        #sub_.append([x[u'start_location'][u'lat'],x[u'start_location'][u'lng']])
        step_len_ = len(x[u'steps'])
        for j in range(step_len_):
            kstep = x[u'steps'][j]
            sub_.append([kstep[u'start_location'][u'lat'],kstep[u'start_location'][u'lng']])
            #sub_.append([kstep[u'end_location'][u'lat'],kstep[u'end_location'][u'lng']])
        sub_.append([x[u'end_location'][u'lat'],x[u'end_location'][u'lng']])
        routes.append(sub_)
    print routes
    print len(routes)
    return routes
if __name__ == "__main__":
    route_ = getpoints()
    for i in route_:
        print stations.getVulnerability(i)


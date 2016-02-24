from django.shortcuts import render
import getPathPoints
import scraper
from django.http import JsonResponse
import json
import stations

# Create your views here.
def get_hawa(request):
    source_lat = float(request.GET['source_lat'])
    print source_lat
    source_long =float(request.GET['source_long'])
    dest_lat = float(request.GET['dest_lat'])
    dest_long = float(request.GET['dest_long'])
    paths = getPathPoints.getpoints(source_lat,source_long,dest_lat,dest_long)
    vuln = stations.getVulnerability(paths[0])
    un = vuln
    ind = 0
    for i in range(1,len(paths)):
        vuln = stations.getVulnerability(paths[i])
        if un>vuln:
            un = vuln
            ind = i
    short_path = paths[ind]
    dicto = {}
    k = 1
    for i in short_path:
        dicto[k] = i
        k+=1
    print '\n',dicto
    return JsonResponse(json.dumps(dicto),safe=False)


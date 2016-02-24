from django.conf.urls import include, url
from django.contrib import admin
from . import views

urlpatterns = [
                url(r'^get/',views.get_hawa,name='get_hawa'),
                ]

